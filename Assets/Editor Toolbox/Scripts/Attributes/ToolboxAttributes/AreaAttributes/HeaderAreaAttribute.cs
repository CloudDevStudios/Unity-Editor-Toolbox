﻿using System;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
public class HeaderAreaAttribute : ToolboxAreaAttribute
{
    public HeaderAreaAttribute(string label, HeaderStyle style = HeaderStyle.Unity)
    {
        Label = label;
        Style = style;
    }

    public string Label { get; private set; }

    public HeaderStyle Style { get; private set; }
}

public enum HeaderStyle
{
    Unity,
    Boxed
}