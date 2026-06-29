| 6 | OpenMBU | main | Beta-1.17.4-64-g9b786101 | robertpelloni/OpenMBU | clean |

## SMB Minigame Asset Requirements (Future/Pending)
- The minigames (Bowling, Billiards, Target) currently use fallback/placeholder geometries (`gem.dts`, `pball_round.dts`) from the native OpenMBU directory.
- Future submodule pulls should map custom shape geometries (e.g. `smb_bowling_pin.dts`, `smb_pool_ball.dts`, `smb_target_ring.dts`) into `~/data/shapes/minigames/` to achieve true visual parity.
