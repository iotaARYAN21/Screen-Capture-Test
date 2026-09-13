# Screen and Video Sharing System Architecture

## 1. Data Capture and Initial Processing — Client-Side Sender

### 1.1 Objective

Capture the screen or video feed at an optimized frame rate, process the raw image data to identify differences from the previous frame, and format the data for network transmission.

### 1.2 Data Sources & Input

#### Screen Capture

A C# API will be utilized to take snapshots of the user's current desktop screen.

#### Video Capture

The core logic for video sharing is treated as identical to screen sharing, utilizing the web camera feed as the image source instead of the desktop screen.

### 1.3 Core Processing Logic — Image Diffing Algorithm

To prevent network bottlenecks and server crashes—which could occur if sending approximately **1 GB of data per second to 30 different machines**—full image frames must not be sent continuously.

The module will implement an **image diffing algorithm**.

#### Step 1: Capture Frame

Retrieve the current state of the screen or camera feed at a set interval.

#### Step 2: Comparison

Compare the current frame against the previously stored frame in local memory.

#### Step 3: Static State — Pulse

If no pixels have changed between frames:

* Halt image transmission.
* Send a lightweight **pulse** signal.
* The pulse informs the network that the screen remains unchanged.

#### Step 4: Delta State — Pixel Extraction

If the screen has changed:

* Identify the pixels that have changed.
* Isolate only the required delta pixel data.
* For example, if a clock changes from `4:59` to `5:00`, only the affected region needs to be transmitted.

#### Step 5: Serialization & Formatting

Convert the captured image or delta data into a format suitable for network transmission.

For example:

* PNG representation for full frames.
* Serialized pixel/region data for delta updates.
* String encoding when required by the networking layer.

### 1.4 Data Structure

The system requires an object-oriented class structure to package pixel updates efficiently before sending them over the network.

| Field           | Type         | Description                                                                                                       |
| --------------- | ------------ | ----------------------------------------------------------------------------------------------------------------- |
| `Timestamp`     | `DateTime`   | The exact time the frame or pulse was captured.                                                                   |
| `IsPulse`       | `Boolean`    | `true` if the screen is unchanged; `false` if pixel data is present.                                              |
| `Resolution`    | `String`     | The source screen resolution, allowing the receiving client to properly scale the image and manage memory limits. |
| `ChangedPixels` | `Array/List` | The specific delta pixel data, including coordinates and color values, when `IsPulse` is `false`.                 |
| `ImageData`     | `String`     | A string representation of the PNG snapshot when a full frame or snapshot is required.                            |

### 1.5 Next-Step Trigger

Once the data object is constructed, it is handed off to the **Networking Module** to be broadcast to the server or peer clients.

The handoff must occur without blocking the UI thread so that the application remains responsive.

---

# 2. Network Reception, Client-Side Stitching, and UI Display

## 2.1 Objective

Receive optimized image data or pulse signals from the network, efficiently reconstruct the image using a stitching algorithm, and render the feeds dynamically in a scalable and interactive UI grid.

## 2.2 Network Reception & View-Based Optimization

To prevent unnecessary network congestion, the client must actively manage and restrict the stream data it receives.

### Selective Fetching — Bandwidth Saving

The client should avoid receiving streams that are not currently being viewed.

For example:

1. A session contains 30 machines.
2. The user selects a single screen to view.
3. The application signals the other machines to stop sending their stream data.
4. Only the selected stream continues to transmit.

This reduces unnecessary bandwidth consumption.

### State Handling

The network module continuously listens for incoming data objects and determines whether each packet contains:

* A **pulse**, indicating that the source frame has not changed.
* **Delta pixel data**, indicating that part of the source frame has changed.
* A **full frame**, used when the visual change is too large for efficient delta transmission.

---

## 2.3 Core Processing Logic — Client-Side Stitching Algorithm

Once data is received, the client must reconstruct the final image before displaying it.

### Pulse Processing

If a pulse is received:

* No image processing is required.
* The UI continues displaying the previously cached image frame.

### Image Stitching

If delta pixels are received, the client uses an **Image Stitcher** object.

The Image Stitcher:

1. Takes the locally cached base image.
2. Applies the newly received changed pixels.
3. Produces the updated image.
4. Sends the updated image to the UI for rendering.

For example, if only the clock changes from `4:59` to `5:00`, the existing cached frame is retained and only the changed region is updated.

### Full-Frame Fallback

The algorithm must account for situations involving significant visual changes.

For example, if the source screen suddenly switches to a live movie or YouTube video:

* A large percentage of pixels may change between frames.
* Sending individual pixel differences may become as expensive as sending the entire image.
* The system should therefore bypass the diffing process.
* A complete image frame should be transmitted instead.

A suitable implementation can use a configurable **change threshold** to determine when delta transmission is no longer efficient.

---

## 2.4 UI Layout & Display Integration

The stitched images are pushed to the user interface, which must gracefully handle multiple concurrent connections.

### Dynamic Grid Structure

The UI displays incoming screen and video shares as small tiles within a dynamic grid.

### Scalability & Scrolling

The grid must support large sessions, such as a class containing **30 students**.

Requirements:

* Maintain a readable minimum tile size.
* Dynamically generate additional rows/columns as required.
* Provide scrolling when the number of feeds exceeds the available screen area.
* Avoid excessive memory usage when handling multiple feeds.

---

## 2.5 User Interactions

### Click-to-Zoom

Users can click on an individual tile within the scrollable grid.

The selected stream then:

1. Expands from its tile.
2. Becomes the primary focus of the UI.
3. Continues displaying the corresponding live feed.

---

## 2.6 Next-Step Trigger

Once the UI successfully renders the live, stitched feeds, the next stage is to implement the event-driven interactions.

These include:

* Incident Management functionality.
* Localized snapshot capture.
* Communication with cloud/external APIs.

---

# 3. Incident Capture and API Integration

## 3.1 Objective & UI Trigger

Enable users to capture critical moments from live video or screen feeds and transmit these snapshots to the **Incident Management System** for logging and analysis.

The capture functionality must be integrated seamlessly into the UI and operate strictly within the **XAML `Grid` and `StackPanel`** provided by the UX team.

### Right-Click Action

An event listener should be implemented on the active video or screen-share tiles.

When the user right-clicks a tile:

* A context menu is displayed.
* The menu provides actions applicable to the selected stream.

### Snapshot Command

The context menu should include a **"Snip"** or **"Snapshot"** option.

When selected:

1. Identify the selected video/screen stream.
2. Retrieve the frame currently rendered by the client.
3. Freeze/capture that frame.
4. Prepare it for transmission to the Incident Management system.

---

## 3.2 Data Preparation & Serialization

Once the user triggers a snapshot—for example, after observing a critical event—the module prepares the image for network transmission.

### In-Memory Capture

Isolate the current stitched frame from client-side memory.

### Format Conversion

Convert the captured frame into a standardized **PNG** format.

### String Serialization

Encode the PNG image into a string representation suitable for HTTP transmission.

### Metadata Tagging

Attach the necessary metadata to the payload, including:

* Exact capture timestamp.
* Session ID.
* Source stream identifier.

This allows the Incident Management system to correlate multiple snapshots belonging to the same event.

---

## 3.3 Module Handoff & API Boundary

The team's responsibility ends at data transmission. The team is completely isolated from database management and downstream incident processing.

### API Transmission

Send the serialized image data and associated metadata directly to the **Incident Manager Module's designated API**.

### Scope Restriction

The **Incident Management Team** is exclusively responsible for downstream processing, including:

* Database storage.
* Incident tracking.
* Incident lifecycle management.
* Assignment of incident IDs.
* Further incident processing and analysis.

The screen/video sharing module does not directly interact with the Incident Management database.

---

## 3.4 Incident API Payload Data Structure

The data object transmitted to the Incident API should have a well-defined structure to ensure clean integration between the modules.

| Field            | Type       | Description                                                       |
| ---------------- | ---------- | ----------------------------------------------------------------- |
| `ImagePayload`   | `String`   | Serialized PNG image data.                                        |
| `CaptureTime`    | `DateTime` | Exact local time at which the snapshot was triggered.             |
| `SourceStreamID` | `String`   | Identifier for the specific video or screen feed being monitored. |

---

# 4. Overall Data Flow

The overall system can be summarized as follows:

```text
┌──────────────────────┐
│ Screen / Camera Feed │
└──────────┬───────────┘
           │
           ▼
┌──────────────────────┐
│ Frame Capture        │
└──────────┬───────────┘
           │
           ▼
┌──────────────────────┐
│ Frame Comparison     │
│ Previous vs Current  │
└──────────┬───────────┘
           │
       ┌───┴───────────────┐
       │                   │
   No Change           Change Detected
       │                   │
       ▼                   ▼
┌─────────────┐     ┌─────────────────┐
│ Pulse       │     │ Delta Extraction│
└──────┬──────┘     └────────┬────────┘
       │                      │
       │                ┌─────┴──────────┐
       │                │ Change >       │
       │                │ Threshold?     │
       │                └─────┬──────────┘
       │                      │
       │                 Yes  │  No
       │                  │    │
       │                  ▼    ▼
       │            ┌────────┐ ┌──────────────┐
       │            │Full    │ │Delta Pixel   │
       │            │Frame   │ │Data          │
       │            └────┬───┘ └──────┬───────┘
       │                 │            │
       └─────────────────┴────────────┘
                         │
                         ▼
                ┌──────────────────┐
                │ Serialization    │
                └────────┬─────────┘
                         │
                         ▼
                ┌──────────────────┐
                │ Network Module   │
                └────────┬─────────┘
                         │
                         ▼
                ┌──────────────────┐
                │ Network Receiver │
                └────────┬─────────┘
                         │
                         ▼
                ┌──────────────────┐
                │ Image Stitcher   │
                └────────┬─────────┘
                         │
                         ▼
                ┌──────────────────┐
                │ Cached Frame     │
                └────────┬─────────┘
                         │
                         ▼
                ┌──────────────────┐
                │ Dynamic UI Grid  │
                └────────┬─────────┘
                         │
                  ┌──────┴───────┐
                  │              │
             View / Zoom    Right Click
                                │
                                ▼
                         ┌────────────┐
                         │ Snapshot   │
                         └─────┬──────┘
                               │
                               ▼
                         ┌────────────┐
                         │ PNG Encode │
                         └─────┬──────┘
                               │
                               ▼
                         ┌────────────┐
                         │ Incident   │
                         │ Manager API│
                         └────────────┘
```

## 5. Module Responsibilities

| Module                         | Primary Responsibility                                            |
| ------------------------------ | ----------------------------------------------------------------- |
| **Capture Module**             | Capture screen/camera frames.                                     |
| **Diffing Module**             | Compare frames and determine changed regions.                     |
| **Serialization Module**       | Convert frame/delta data into network-ready objects.              |
| **Networking Module**          | Transmit and receive stream data.                                 |
| **Image Stitcher**             | Reconstruct frames from cached images and delta updates.          |
| **UI Module**                  | Display feeds in a dynamic grid and provide user interactions.    |
| **Snapshot Module**            | Capture the currently rendered stream frame.                      |
| **Incident API Integration**   | Serialize and transmit snapshot data to the Incident Manager API. |
| **Incident Management Module** | Store, track, and process incidents downstream of the API.        |

## 6. Key Design Principles

1. **Minimize network bandwidth** by avoiding unnecessary full-frame transmission.
2. **Use pulse messages** when the source frame has not changed.
3. **Use delta updates** when only a small portion of the image changes.
4. **Use full-frame transmission** when the changed region becomes too large.
5. **Cache the latest complete frame** on the receiving client.
6. **Keep UI operations responsive** by avoiding blocking operations on the UI thread.
7. **Use selective stream transmission** to avoid receiving feeds that are not required.
8. **Support multiple concurrent streams** through a scalable grid-based UI.
9. **Separate responsibilities** between streaming, UI, and Incident Management modules.
10. **Keep database operations outside the streaming module**, with all incident persistence handled by the Incident Management team.
11. **Maintain clear API boundaries** between the streaming client and Incident Management system.
12. **Attach timestamps and stream identifiers** to snapshot payloads for event correlation.
