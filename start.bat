@echo off
setlocal
title OpenMBU (CMake build)
cd /d "%~dp0"

echo [OpenMBU (CMake build)] Starting...
where cmake >nul 2>nul
if errorlevel 1 (
    echo [OpenMBU (CMake build)] cmake not found. Please install it.
    pause
    exit /b 1
)

cmake --build build --config Release

if errorlevel 1 (
    echo [OpenMBU (CMake build)] Exited with error code %errorlevel%.
    pause
)
endlocal
