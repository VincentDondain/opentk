#region License
//
// The Open Toolkit Library License
//
// Copyright (c) 2006 - 2009 the Open Toolkit library.
// Copyright 2013 Xamarin Inc
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights to 
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do
// so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
//
#endregion

using System;
using System.Collections.Generic;
using System.Text;

namespace OpenTK.Platform
{
    using Graphics;

    sealed class Factory : IPlatformFactory
    {
        #region Fields

        static IPlatformFactory default_implementation, embedded_implementation;

        #endregion

        #region Constructors

        static Factory()
        {
#if MOBILE
            if (Configuration.RunningOnAndroid) Default = new Android.AndroidFactory ();
#else
            if (Configuration.RunningOnWindows) Default = new Windows.WinFactory();
            else if (Configuration.RunningOnMacOS) Default = new MacOS.MacOSFactory();
            else if (Configuration.RunningOnX11) Default = new X11.X11Factory();
#endif
            else Default = new UnsupportedPlatform();
            if (Egl.Egl.IsSupported)
            {
#if MOBILE
                if (Configuration.RunningOnAndroid) Embedded = new Egl.EglAndroidPlatformFactory ();
#else
                if (Configuration.RunningOnWindows) Embedded = new Egl.EglWinPlatformFactory();
                else if (Configuration.RunningOnMacOS) Embedded = new Egl.EglMacPlatformFactory();
                else if (Configuration.RunningOnX11) Embedded = new Egl.EglX11PlatformFactory();
#endif
                else Embedded = new UnsupportedPlatform();
            }
            else Embedded = new UnsupportedPlatform();

            if (Default is UnsupportedPlatform && !(Embedded is UnsupportedPlatform))
                Default = Embedded;
        }

        #endregion

        #region Public Members

        public static IPlatformFactory Default
        {
            get { return default_implementation; }
            private set { default_implementation = value; }
        }

        public static IPlatformFactory Embedded
        {
            get { return embedded_implementation; }
            private set { embedded_implementation = value; }
        }

        #endregion

        #region IPlatformFactory Members

        public INativeWindow CreateNativeWindow(int x, int y, int width, int height, string title,
            GraphicsMode mode, GameWindowFlags options, DisplayDevice device)
        {
            return default_implementation.CreateNativeWindow(x, y, width, height, title, mode, options, device);
        }

        public IDisplayDeviceDriver CreateDisplayDeviceDriver()
        {
            return default_implementation.CreateDisplayDeviceDriver();
        }

        public IGraphicsContext CreateGLContext(GraphicsMode mode, IWindowInfo window, IGraphicsContext shareContext, bool directRendering, int major, int minor, GraphicsContextFlags flags)
        {
            return default_implementation.CreateGLContext(mode, window, shareContext, directRendering, major, minor, flags);
        }

        public IGraphicsContext CreateGLContext(ContextHandle handle, IWindowInfo window, IGraphicsContext shareContext, bool directRendering, int major, int minor, GraphicsContextFlags flags)
        {
            return default_implementation.CreateGLContext(handle, window, shareContext, directRendering, major, minor, flags);
        }

        public GraphicsContext.GetCurrentContextDelegate CreateGetCurrentGraphicsContext()
        {
            return default_implementation.CreateGetCurrentGraphicsContext();
        }

        public IGraphicsMode CreateGraphicsMode()
        {
            return default_implementation.CreateGraphicsMode();
        }
#if !IPHONE
        public OpenTK.Input.IKeyboardDriver2 CreateKeyboardDriver()
        {
            return default_implementation.CreateKeyboardDriver();
        }

        public OpenTK.Input.IMouseDriver2 CreateMouseDriver()
        {
            return default_implementation.CreateMouseDriver();
        }
#endif
        class UnsupportedPlatform : IPlatformFactory
        {
            #region Fields
            
            static readonly string error_string = "Please, refer to http://www.opentk.com for more information.";
            
            #endregion
            
            #region IPlatformFactory Members

            public INativeWindow CreateNativeWindow(int x, int y, int width, int height, string title, GraphicsMode mode, GameWindowFlags options, DisplayDevice device)
            {
                throw new PlatformNotSupportedException(error_string);
            }

            public IDisplayDeviceDriver CreateDisplayDeviceDriver()
            {
                throw new PlatformNotSupportedException(error_string);
            }

            public IGraphicsContext CreateGLContext(GraphicsMode mode, IWindowInfo window, IGraphicsContext shareContext, bool directRendering, int major, int minor, GraphicsContextFlags flags)
            {
                throw new PlatformNotSupportedException(error_string);
            }

            public IGraphicsContext CreateGLContext(ContextHandle handle, IWindowInfo window, IGraphicsContext shareContext, bool directRendering, int major, int minor, GraphicsContextFlags flags)
            {
                throw new PlatformNotSupportedException(error_string);
            }

            public IGraphicsContext CreateESContext(GraphicsMode mode, IWindowInfo window, IGraphicsContext shareContext, int major, int minor, GraphicsContextFlags flags)
            {
                throw new PlatformNotSupportedException(error_string);
            }

            public GraphicsContext.GetCurrentContextDelegate CreateGetCurrentGraphicsContext()
            {
                throw new PlatformNotSupportedException(error_string);
            }

            public IGraphicsMode CreateGraphicsMode()
            {
                throw new PlatformNotSupportedException(error_string);
            }

            public OpenTK.Input.IKeyboardDriver2 CreateKeyboardDriver()
            {
                throw new PlatformNotSupportedException(error_string);
            }

            public OpenTK.Input.IMouseDriver2 CreateMouseDriver()
            {
                throw new PlatformNotSupportedException(error_string);
            }
            
            #endregion
        }

        #endregion
    }
}
