﻿// SPDX-FileCopyrightText: 2023 Frans van Dorsselaer
//
// SPDX-License-Identifier: GPL-3.0-only

using System.CommandLine.IO;
using Usbipd.Automation;

namespace UnitTests;

[TestClass]
sealed class ConsoleTools_Tests
{
    [TestMethod]
    [DataRow("", 10, new[] { "" })]
    [DataRow("1", 10, new[] { "1" })]
    [DataRow("123456789", 10, new[] { "123456789" })]
    [DataRow("123456789a", 10, new[] { "123456789a " })]
    [DataRow("123456789ab", 10, new[] { "123456789ab" })]
    [DataRow("12345 789", 10, new[] { "12345 789" })]
    [DataRow("12345 789a", 10, new[] { "12345", "789a" })]
    public void Wrap(string input, int width, string[] expected)
    {
        var result = ConsoleTools.Wrap(input, width);
        CollectionAssert.AreEquivalent(expected, new List<string>(result));
    }


    [TestMethod]
    public void ReportError()
    {
        var console = new TestConsole();
        ConsoleTools.ReportError(console, "test");
        Assert.IsTrue(string.IsNullOrEmpty(console.Out.ToString()));
        Assert.IsFalse(string.IsNullOrEmpty(console.Error.ToString()));
    }

    [TestMethod]
    public void ReportWarning()
    {
        var console = new TestConsole();
        ConsoleTools.ReportWarning(console, "test");
        Assert.IsTrue(string.IsNullOrEmpty(console.Out.ToString()));
        Assert.IsFalse(string.IsNullOrEmpty(console.Error.ToString()));
    }

    [TestMethod]
    public void ReportInfo()
    {
        var console = new TestConsole();
        ConsoleTools.ReportInfo(console, "test");
        Assert.IsTrue(string.IsNullOrEmpty(console.Out.ToString()));
        Assert.IsFalse(string.IsNullOrEmpty(console.Error.ToString()));
    }

    [TestMethod]
    public void ReportVersionNotSupported()
    {
        var console = new TestConsole();
        ConsoleTools.ReportVersionNotSupported(console, new());
        Assert.IsTrue(string.IsNullOrEmpty(console.Out.ToString()));
        Assert.IsFalse(string.IsNullOrEmpty(console.Error.ToString()));
    }

    [TestMethod]
    public void ReportRebootRequired()
    {
        var console = new TestConsole();
        ConsoleTools.ReportRebootRequired(console);
        Assert.IsTrue(string.IsNullOrEmpty(console.Out.ToString()));
        Assert.IsFalse(string.IsNullOrEmpty(console.Error.ToString()));
    }

    [TestMethod]
    [DataRow("VID_80EE&PID_CAFE", false)]
    [DataRow("VID_12AB&PID_34CD", true)]

    public void CheckNoStub(string hardwareId, bool nostub)
    {
        var vidpid = VidPid.FromHardwareOrInstanceId(hardwareId);
        var console = new TestConsole();
        if (nostub)
        {
            Assert.IsTrue(ConsoleTools.CheckNoStub(vidpid, console));
            Assert.IsTrue(string.IsNullOrEmpty(console.Out.ToString()));
            Assert.IsTrue(string.IsNullOrEmpty(console.Error.ToString()));
        }
        else
        {
            Assert.IsFalse(ConsoleTools.CheckNoStub(vidpid, console));
            Assert.IsTrue(string.IsNullOrEmpty(console.Out.ToString()));
            Assert.IsFalse(string.IsNullOrEmpty(console.Error.ToString()));
        }
    }
}
