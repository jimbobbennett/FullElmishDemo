namespace FullElmishDemo.Tests

open FullElmishDemo
open Elmish.XamarinForms
open Elmish.XamarinForms.DynamicViews
open NUnit.Framework
open FsUnit

module Test =
    let getElementFromName<'T> name (parent : ViewElement) =
        let (c : ValueOption<'T>) = parent.TryGetAttribute name
        match c with
        | ValueSome x -> x
        | ValueNone -> raise (System.Exception(sprintf "Attribute %s not found" name))

    [<Test>]
    let ``Test init``() =
        let i = App.init()
        let m = fst i
        let c = snd i

        m.Count |> should equal 0
        c |> should equal Cmd.none
        ()

    [<Test>]
    let ``Test update with increment``() =
        let m = App.init() |> fst
        let newM = App.update App.Msg.Increment m |> fst

        m.Count |> should equal 0
        newM.Count |> should equal 1

    [<Test>]
    let ``Test update with Go``() =
        let m = App.init() |> fst
        let newM = App.update App.Msg.Go m |> fst

        m.ShowWelcome |> should equal true
        newM.ShowWelcome |> should equal false

    [<Test>]
    let ``Test view returns the right view``() =
        let m = { App.Model.Count = 27; App.Model.ShowWelcome = false}
        App.view m ignore 
            |> getElementFromName<ViewElement[]> "Pages"
            |> Array.head
            |> getElementFromName<ViewElement> "Content"
            |> getElementFromName<ViewElement[]> "Children"
            |> Array.find (fun x -> x.Create().GetType() = typedefof<Xamarin.Forms.Label>)
            |> getElementFromName<string> "Text"
            |> should equal "Count: 27"

    [<TestCase(false, 1)>]
    [<TestCase(true, 2)>]
    let ``Test show welcome returns the right number of pages`` s l =
        let m = { App.Model.Count = 27; App.Model.ShowWelcome = s}
        App.view m ignore 
            |> getElementFromName<ViewElement[]> "Pages"
            |> Array.length
            |> should equal l
