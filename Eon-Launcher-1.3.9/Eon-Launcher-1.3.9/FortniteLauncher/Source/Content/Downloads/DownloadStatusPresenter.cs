using System;
using Microsoft.UI.Xaml;
using FortniteLauncher.Pages;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;

class DownloadStatusPresenter
{
    private static readonly Random Randomizer = new();

    public static void ShowMessage()
    {
        string Message = Text.DownloadMessages[Randomizer.Next(Text.DownloadMessages.Length)];

        var MessageRun = new Run { Text = Message, FontSize = 14 };
        var MessageParagraph = new Paragraph();
        MessageParagraph.Inlines.Add(MessageRun);

        var MessageBlock = new RichTextBlock();
        MessageBlock.Blocks.Add(MessageParagraph);

        PlayPage.Launch_Button.Header = string.Empty;
        PlayPage.Launch_Button.Description = MessageBlock;

        StartLoadingAnimation(MessageRun, Message);
    }

    private static void StartLoadingAnimation(Run MessageRun, string BaseMessage)
    {
        int DotCount = 0;
        var Timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.5) };

        Timer.Tick += (Sender, Event) =>
        {
            DotCount = (DotCount + 1) % 4;
            MessageRun.Text = BaseMessage + new string('.', DotCount);
        };

        Timer.Start();
    }
}