LimE.Mail.Builders
=============

Mail templating and utility library for aspent core services and applications.

![Nuget](https://img.shields.io/nuget/v/LimE.Mail.Builders)
![CircleCI](https://img.shields.io/circleci/build/gh/lime-it/LimE.Mail.Builders/master)
![Nuget](https://img.shields.io/nuget/dt/LimE.Mail.Builders)
![GitHub](https://img.shields.io/github/license/lime-it/LimE.Mail.Builders)

# Usage

Register DI providers

#### `Startup.cs`
``` csharp

services.AddMailTemplateBuilders();

```

Create a razor view as a mail tempalte:

#### `Example-0.cshtml`
``` csharp

Hello to @(!ViewData.ContainsKey("name") ? "everybody" : ViewData["name"])!

```

Render a mail body in code:


#### `SendMail.cs`
``` csharp

var values = new Dictionary<string, string>()
{
    { "name", "test" }
};

var viewRenderer = provider.GetRequiredService<ViewTemplateRenderer>();

var result = await viewRenderer.RenderViewAsync("Example-0", values.AsViewData(), ViewRendererTargetFormat.Html);

```