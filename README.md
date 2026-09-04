# osi.net

**osi.net** (Operating System Interface) is a comprehensive, modular, cross-platform framework written in VB.NET.

The core design philosophy of `osi.net` is to **decouple business logic from I/O and underlying runtime environments**, allowing developers to write high-performance logic once and run it across diverse platforms—from local desktops and servers to distributed services and cross-platform runtimes.

---

## Target Frameworks & Platforms

`osi.net` is dual-targeted to support both modern cross-platform .NET environments and legacy runtimes:

- **Modern .NET**: .NET 8.0 / .NET 10.0 on Linux and Windows.
- **Legacy .NET Framework**: .NET Framework 4.0 on Windows.

---

## Repository Architecture

The codebase is organized into three primary layers:

### 1. `root/` — Core Foundation Layer
The foundation library providing essential runtime utilities, primitives, and testing infrastructure:
- **`connector/`**: Core system wrappers, reflection helpers, memory manipulation, LOH (Large Object Heap) compaction, and functor registries.
- **`constants/`**: System-wide constants, limits, and type definitions.
- **`delegates/`**: Fast delegate wrappers, argument parsers, and function bindings.
- **`envs/`**: Hardware, OS, processor detection, environment variables, and system performance metrics.
- **`event/`**: Lightweight event mechanisms, signals, and synchronizers.
- **`formation/`**: High-performance data structures including lock-free queues, ring buffers, balanced trees, and thread-safe unique maps.
- **`lock/`**: Extensive concurrency primitives (atomic variables, spinlocks, reader-writer locks, lazy locks).
- **`procedure/`**: Procedure-based asynchronous programming modules (`event_comb`), coroutines, and callback managers.
- **`template/`**: Type templates, conversions, and type traits.
- **`threadpool/`**: High-performance custom thread pools (`slimqless`, `qless`, `heapless`) interchangeable with the standard managed thread pool.
- **`utils/`**: Diagnostics, counters, structured loggers, stopwatches, and unhandled exception handlers.
- **`utt/`**: The built-in Unit Test Tool (`osi.root.utt`) framework with support for concurrency scheduling, processor reservation, and isolated process execution.
- **`tests/`**: Comprehensive unit tests covering all `root` modules.

### 2. `service/` — Infrastructure & Service Layer
High-level services and building blocks built on top of the root layer:
- **Networking & Transmitters**: High-throughput TCP (`service/tcp`), UDP (`service/udp`), HTTP server and client (`service/http`), and shared transmitter multiplexing (`service/sharedtransmitter`).
- **Data & Streaming**: Streamers and pipelines (`service/streamer`), data providers with caching and file monitoring (`service/dataprovider`), and virtual storage engines (`service/storage`, `service/webstorage`).
- **Computing & Language**: Scripting/interpreter engine (`service/interpreter`, `service/compiler`), dynamic logic evaluation (`service/dynamiclogic`), and arbitrary precision math (`service/math`).
- **Resource Management**: Object pools, device pools, caching managers, and DoS protectors (`service/devicepool`, `service/cache`, `service/protector`).
- **`service/tests/`**: Exhaustive test suites for all services.

### 3. `production/` — Utilities & Applications
Standalone applications, diagnostic utilities, and server bridges:
- `test_http_server`, `tcp_bridge`, `tcp_pair`, `http_proxy`, `remote_console`, `big_int_calculator`, `big_uint_calculator`, `sider`, `b2style`, and `utt_diff`.

---

## Key Concepts & Highlights

- **`event_comb` Asynchronous Model**: Write complex, non-blocking asynchronous workflows, multi-step state machines, and I/O operations in a sequential, structured manner without callback hell.
- **Memory & LOH Optimization**: Proactive Large Object Heap (LOH) compaction and processor-aware resource scheduling to keep memory fragmentation and RAM footprint bounded across thousands of concurrent executions.
- **High-Performance Thread Pooling**: Lightweight, minimal-overhead thread pools designed for low-latency task dispatching.
- **Flexible Type & Serialization System**: Self-adapting comparers without rigid `IComparable` requirements, dynamic object construction, and high-performance binary/string serializers.

---

## Building

### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/) 8.0 or later (tested with .NET SDK 10.0).
- Bash shell (Linux, macOS, or WSL / Git Bash on Windows).

### Build Instructions
To build all projects across `root`, `service`, and `production`:

```bash
# Source environment variables (sets up local dotnet paths if needed)
source setenv.sh

# Build all .NET projects
./build.sh
```

---

## Running Tests

`osi.net` includes an extensive test suite consisting of over 1,600 test cases executed via `osi.root.utt`.

### Run the Complete Test Suite
```bash
./run-utt.sh
```

### Run Specific Test Cases
You can filter tests by case name:

```bash
./run-utt.sh --case=<case_name>

# Example:
./run-utt.sh --case=gc_behavior_test
```

---

## License & Notice

```text
***************************************************************
Non-commercial use only.
Please contact the author for commercial licensing,
except for companies the author is currently or was previously working at.
***************************************************************
```

For questions or inquiries, contact: **hzj_jie@hotmail.com**
