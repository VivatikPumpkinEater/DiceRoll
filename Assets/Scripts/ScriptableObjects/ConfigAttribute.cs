using System;

/// <summary> Атрибут, чтобы указать путь конфига </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ConfigAttribute : Attribute
{
    public string FilePath;
}