﻿using System;
using System.Windows.Input;

namespace ProgLangDownloader;

public class RelayCommand : ICommand
{
    private readonly Action _execute;

    public RelayCommand(Action execute)
    {
        _execute = execute;
    }

    public event EventHandler? CanExecuteChanged = delegate { };

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter) => _execute();
}

 