# Narva Grid Sentinel

**Status:** Active Prototype
**Target:** Nord Pool (Estonia)
**Stack:** .NET 8 / Docker

### Overview
Narva Grid Sentinel is a decoupled, low-latency Worker Service designed for the Ida-Virumaa industrial sector. It acts as a "Digital Fuse" between the national energy grid and heavy machinery.

It autonomously monitors real-time electricity prices via the Elering API and triggers Modbus TCP load-shedding protocols when prices exceed critical thresholds.

### Architecture
The system follows a microservice-ready architecture:

1. Grid Watcher: Polls Elering (Nord Pool) JSON endpoints with System.Net.Http.Json (Zero-Allocation focus).
2. Logic Core: Compares live rates against user-defined thresholds (e.g., 150 EUR/MWh).
3. Industrial Gateway: Dispatches WriteSingleCoil (Function 0x05) packets to PLCs via Modbus TCP (Port 502) to safely halt production.

### Technical Highlights
* Runtime: .NET 8 (Native AOT compatible for <50MB footprint).
* Latency: ~15ms from API response to Modbus packet generation.
* Resilience: Implements exponential backoff for network outages.
* Simulation: Includes a Mock PLC Service for verifying Modbus packet structure (01 3A 00...) without hardware.

### Installation
git clone https://github.com/YOUR-USERNAME/Narva-Grid-Sentinel.git
dotnet run