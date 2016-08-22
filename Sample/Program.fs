(*
** F# GLFW binding
** Copyright (C) 2015-2016 Wael El Oraiby
** 
** This program is free software: you can redistribute it and/or modify
** it under the terms of the GNU Affero General Public License as
** published by the Free Software Foundation, either version 3 of the
** License, or (at your option) any later version.
** 
** This program is distributed in the hope that it will be useful,
** but WITHOUT ANY WARRANTY; without even the implied warranty of
** MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
** GNU Affero General Public License for more details.
** 
** You should have received a copy of the GNU Affero General Public License
** along with this program.  If not, see <http://www.gnu.org/licenses/>.
*)
[<EntryPoint>]
let main argv = 

    printfn "init: %d" (Glfw3.init())
    printfn "version: %A" (Glfw3.getVersion())
    printfn "str version: %s" (Glfw3.getVersionString())

    let monitors = Glfw3.getMonitors()
    printfn "monitors: %d" (monitors.Length)

    let primaryMonitor = Glfw3.getPrimaryMonitor()

    printfn "primary monitor position: %A" (Glfw3.getMonitorPos(primaryMonitor))

    monitors
    |> Array.iter (fun m -> printfn "Name: %s, Pos: %A, Size: %A" (Glfw3.getMonitorName m) (Glfw3.getMonitorPos m) (Glfw3.getMonitorPhysicalSize m))

    monitors
    |> Array.iteri
        (fun im m ->
            m
            |> Glfw3.getVideoModes
            |> Array.iteri
                (fun iv vm ->
                    printfn "%dx%d - width:  %d - height: %d - RGB:    %d%d%d - rate:   %d" im iv vm.width vm.height vm.redBits vm.greenBits vm.blueBits vm.refreshRate))

    let vm = Glfw3.getVideoMode primaryMonitor
    printfn "Primary: width:  %d - height: %d - RGB:    %d%d%d - rate:   %d" vm.width vm.height vm.redBits vm.greenBits vm.blueBits vm.refreshRate    

    //Glfw3.setGamma(primaryMonitor, 1.0f)

    let gammaRamp0 = Glfw3.getGammaramp primaryMonitor
    Glfw3.setGammaramp(primaryMonitor, gammaRamp0)
    let gammaRamp1 = Glfw3.getGammaramp primaryMonitor

    gammaRamp0.Red
    |> Array.zip gammaRamp1.Red
    |> Array.iter(fun (r0, r1) -> printfn "r0: %d - r1: %d" r0 r1)

    let win = Glfw3.createWindow(640, 480, "Hello World", None, None)

    Glfw3.setWindowTitle(win, "Hahaha")
    Glfw3.setWindowPos(win, 100, 120)
    Glfw3.setWindowSize(win, 512, 512)

    printfn "Framebuffer %A" (Glfw3.getFrameBufferSize win)
    printfn "Window Frame size %A" (Glfw3.getWindowFrameSize win)

    Glfw3.iconifyWindow win
    Glfw3.restoreWindow win
    Glfw3.hideWindow win
    Glfw3.showWindow win

    let rnd = System.Random ()

    let windowRefresh win =
        printfn "refresh: %d" (rnd.Next())
        //GLES2.glClear ((GLenum.GL_COLOR_BUFFER_BIT ||| GLenum.GL_DEPTH_BUFFER_BIT) |> uint32)
        Glfw3.swapBuffers win

    Glfw3.setWindowRefreshCallback(win, windowRefresh)                                                                                              |> ignore
    Glfw3.setWindowSizeCallback (win, fun (win, w, h) -> printfn "w: %d, h: %d" w h)                                                                |> ignore
    Glfw3.setWindowPosCallback  (win, fun (win, x, y) -> printfn "x: %d, y: %d" x y)                                                                |> ignore
    Glfw3.setWindowFocusCallback(win, fun (win, b) -> if b then printfn "focused" else printfn "unfocused")                                         |> ignore
    Glfw3.setWindowIconifyCallback (win, fun (win, b) -> if b then printfn "iconified" else printfn "uniconified")                                  |> ignore
    Glfw3.setFramebufferSizeCallback (win, fun (win, w, h) -> printfn "FB: w: %d, h: %d" w h)                                                       |> ignore
    Glfw3.setKeyCallback(win, fun (w, k, i, a, m) -> printfn "%A - %d - %A - %A" k i a m; printfn "Key State: %A" (Glfw3.getKey (w, k)))            |> ignore
    Glfw3.setCharCallback(win, fun (w, c) -> printfn "%c" c)                                                                                        |> ignore
    Glfw3.setCharModsCallback(win, fun (w, c, m) -> printfn "%c - %A" c m)                                                                          |> ignore
    Glfw3.setMouseButtonCallback(win, fun (w, b, a, m) -> printfn "mouse %A, %A, %A" b a m; printfn "clipboard: %s" (Glfw3.getClipboardString w))   |> ignore
//    Glfw3.setCursorPosCallback(win, fun (w, x, y) -> printfn "pos: %f, %f" x y)                                                                   |> ignore
    Glfw3.setCursorEnterCallback(win, fun (w, b) -> printfn "Enter: %b" b)                                                                          |> ignore
    Glfw3.setScrollCallback(win, fun (w, x, y) -> printfn "Scroll: %f %f" x y)                                                                      |> ignore
    Glfw3.setDropCallback(win, fun (w, s) -> printfn "%A" s)                                                                                        |> ignore

    let rec loop () =
        if Glfw3.windowShouldClose win
        then ()
        else

            Glfw3.pollEvents ()
            loop ()

    loop ()
    printfn "Pos : %A" (Glfw3.getWindowPos win)
    printfn "Size: %A" (Glfw3.getWindowSize win)

    0 // return an integer exit code
