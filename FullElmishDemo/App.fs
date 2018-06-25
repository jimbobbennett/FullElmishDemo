namespace FullElmishDemo

open Xamarin.Forms
open Elmish.XamarinForms
open Elmish.XamarinForms.DynamicViews

module App =

    type Model =
        {
            ShowWelcome : bool
            Count : int
        }

    type Msg =
        | Go
        | Increment

    let init() =
        { Count = 0; ShowWelcome = true }, Cmd.none

    let update msg model=
        match msg with
        | Increment -> { model with Count = model.Count + 1 }, Cmd.none
        | Go -> { model with ShowWelcome = false }, Cmd.none

    let view model dispatch =
        Xaml.NavigationPage(
            pages = [
                yield Xaml.ContentPage (
                                padding = 20.,
                                content = Xaml.StackLayout(
                                    horizontalOptions = LayoutOptions.Center,
                                    verticalOptions = LayoutOptions.Center,
                                    children = [
                                        Xaml.Label(
                                            text = sprintf "Count: %d" model.Count
                                        )
                                        Xaml.Button(
                                            text = "Increment",
                                            command = (fun () -> dispatch Increment)
                                        )
                                    ]
                                )
                            )
                if (model.ShowWelcome) then
                    yield Xaml.ContentPage (
                                    padding = 20.,
                                    content = Xaml.StackLayout(
                                        horizontalOptions = LayoutOptions.Center,
                                        verticalOptions = LayoutOptions.Center,
                                        children = [
                                            Xaml.Label(
                                                text = "Welcome to Elmish"
                                            )
                                            Xaml.Button(
                                                text = "Go",
                                                command = (fun () -> dispatch Go)
                                            )
                                        ]
                                    )
                                )
            ]
        ) 

type App() as app =
    inherit Application()

    let program = Program.mkProgram App.init App.update App.view
    let runner = 
        program
        |> Program.withConsoleTrace
        |> Program.withDynamicView app
        |> Program.run

   