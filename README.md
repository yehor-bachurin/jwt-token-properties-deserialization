# Jwt Token Properties Deserialization

Benchmark of different types of JWT token deserialization.

| Method                         | Mean       | Error    | StdDev    |
|--------------------------------|------------|----------|-----------|
| AutoDeserialization            | 894.4 us   | 17.85 us | 35.24 us  |
| ManualDeserialization          | 4,455.9 us | 88.19 us | 167.79 us |
| OptimizedManualDeserialization | 917.3 us   | 17.75 us | 47.69 us  |
