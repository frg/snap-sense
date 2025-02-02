// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Drawing;

namespace SnapSense.FaceDetection;

public record DetectedObject(Rectangle Position, double Confidence);
