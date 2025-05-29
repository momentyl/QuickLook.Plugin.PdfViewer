// Copyright © 2017-2025 QL-Win Contributors
//
// This file is part of QuickLook program.
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using QuickLook.Common.Plugin;
using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace QuickLook.Plugin.PDFViewerNative;

public class Plugin : IViewer
{
    private static double _width = 1000;
    private static double _height = 1200;

    public int Priority => 1;

    private WebpagePanel _panel;

    public void Init()
    {
    }

    public bool CanHandle(string path)
    {
        if (File.Exists(path) && Path.GetExtension(path).Equals(".pdf", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }
        return false;
    }

    public void Prepare(string path, ContextObject context)
    {
        context.SetPreferredSizeFit(new Size(_width, _height), 0.9d);
    }

    public void View(string path, ContextObject context)
    {
        _panel = new WebpagePanel();
        context.ViewerContent = _panel;
        context.Title = Path.GetFileName(path);

        _panel.NavigateToFile(path);
        _panel.Dispatcher.Invoke(() => { context.IsBusy = false; }, DispatcherPriority.Loaded);
    }

    public void Cleanup()
    {
        _width = _panel.ActualWidth;
        _height = _panel.ActualHeight;

        _panel?.Dispose();
        _panel = null;

        GC.SuppressFinalize(this);
    }
}
