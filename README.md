<h1 align="center">InputListener</h1>

<p align="center"><em>Handy minimalistic input listening utility with a simple interface</em></p>

## ✅ Why choose InputListener?

If you need an *easy-to-use*, *lightweight*, *well-documented* input listening utility with *helper services* for working with that input,
then **InputListener** is the way to go. It doesn't need any overcomplicated dependencies to work and will be up and running right after the short installation.

## 💎 Features

> #### 🪢 **Doesn't** require any additional dependencies  
> #### 🕹 **Extremely simple** interface  
> #### 🔌 **Easy-pluggable** custom input handlers  
> #### 🔧 **Auxiliary utilities** facilitate working with the received input  
> #### 📘 **Documentation** is nice and clear  

## 💿 Installation

[Download page](https://www.nuget.org/packages/InputListener/)

For guidance on how to install NuGet packages refer [here](https://docs.microsoft.com/en-us/nuget/quickstart/install-and-use-a-package-using-the-dotnet-cli) and [here](https://docs.microsoft.com/en-us/nuget/quickstart/install-and-use-a-package-in-visual-studio).

## 🔩 Usage

```jsx
using InputListener;
using InputListener.Translation;

var listener = new KeyboardListener();
// Represents a method that translates raw input to any type (e.g. string).
Func<ConsoleKey, string> translator = (input) => input.ToString();

// Represents a method that handles input received from the listener.
EventHandler<ConsoleKey> receiver = (object? _, ConsoleKey input) =>
{
    // TranslateWith is one of the helper methods provided by this library.
    string inputTranslatedToString = input.TranslateWith(translator);
    Console.WriteLine(inputTranslatedToString);
};
listener.AttachInputReceiver(receiver);
```

## 💡 Suggestions

This project is a work in progress. If you would like to see some functionality that isn't provided by this library yet, 
feel free to leave your proposals in [**Issues**](https://github.com/GualaBanana/InputListener/issues) section.  Any feedback is highly appreciated.
