# Jwt Token Properties Deserialization

Benchmark of different types of JWT token deserialization.

| Method                         | Mean     | Error     | StdDev    | Median   | Gen0     | Gen1     | Allocated  |
|--------------------------------|----------|-----------|-----------|----------|----------|----------|------------|
| AutoDeserialization            | 1.098 ms | 0.0244 ms | 0.0672 ms | 1.077 ms | 164.0625 | 97.6563  | 792.25 KB  |
| ManualDeserialization          | 5.252 ms | 0.0973 ms | 0.0812 ms | 5.261 ms | 781.2500 | 546.8750 | 3932.57 KB |
| OptimizedManualDeserialization | 1.064 ms | 0.0193 ms | 0.0333 ms | 1.062 ms | 156.2500 | 101.5625 | 786.73 KB  |
