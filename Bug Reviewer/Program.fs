// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open FSharp.Data

type Bugs =  CsvProvider<"csv.csv">

let openURLInBrowser (url:string) =
        System.Diagnostics.Process.Start(url)


let splitName (text:string) =
    text.Split ([|"-All";"-Back";"-Front";"-Design"|], System.StringSplitOptions.None)
    |> Seq.head 

let removeSpaces (a:string) =
    let first = 
        a.Split ([|" "|], System.StringSplitOptions.None)
        |> Seq.map(fun x -> x + "-")
        |> System.String.Concat
    let second =
        first.Split ([|"---"|], System.StringSplitOptions.None)
        |> Seq.map(fun x -> x + "-")
        |> System.String.Concat
    second.Split ([|"."|], System.StringSplitOptions.None)
    |> System.String.Concat

let writeInColour (text : string) (colour : System.ConsoleColor) =
    System.Console.ForegroundColor <- colour
    System.Console.Write text
    System.Console.ResetColor()

[<EntryPoint>]
let main argv =

    let ticket = argv |> Seq.head |> System.IO.Path.GetFileName |> removeSpaces |> splitName
    let bugList = Bugs.Load(argv |> Seq.head)
    printfn "Hit return to open next page\n"
    for row in bugList.Rows |> Seq.tail do
        openURLInBrowser ("http://codebase.enjoy-digital.co.uk/projects/" + ticket + "/tickets/" + row.``#``.ToString())
        |> ignore
        //printfn "Ticket is a %s and is %s in urgancy.\n It's current status is %s" row.Type row.Priority row.Status
        writeInColour "\nTicket is a " System.ConsoleColor.White
        writeInColour row.Type System.ConsoleColor.Red
        writeInColour " and is " System.ConsoleColor.White
        writeInColour row.Priority System.ConsoleColor.Red
        writeInColour " in urgancy. \nIt's current status is " System.ConsoleColor.White
        writeInColour row.Status System.ConsoleColor.Red

        System.Console.ReadLine()
        |> ignore
    0


