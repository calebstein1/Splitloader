using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ReactiveUI;
using Splitloader.UI.Models;
using Splitloader.VideoTools;

namespace Splitloader.UI.ViewModels;

public class SplitloaderViewModel : ViewModelBase
{
    internal readonly FFmpegTools Ffmpeg = new();
    internal string? OutputName { get; set; }
    
    internal SplitloaderViewModel()
    {
        Ffmpeg.FfStatus.PropertyChanged += (sender, e) =>
            Status = Ffmpeg.FfStatus.Value;
        Ffmpeg.ConcatStatus.PropertyChanged += (sender, e) =>
            UiTools.DisplayConcatResult(sender, e, this);
        Task.Run(() => Ffmpeg.FindOrDownloadAsync());
    }

    private ObservableCollection<SelectedFile> _selectedFiles = [];
    internal ObservableCollection<SelectedFile> SelectedFiles
    {
        get => _selectedFiles;
        set => this.RaiseAndSetIfChanged(ref _selectedFiles, value);
    }
    
    private string? _status = "Error initializing FFmpeg. Please restart app.";
    public string? Status
    {
        get => _status;
        set => this.RaiseAndSetIfChanged(ref _status, value);
    }

    public ReactiveCommand<SplitloaderViewModel, Task> SelectFileCommand { get; } =
        ReactiveCommand.Create<SplitloaderViewModel, Task>(UiTools.SelectFileAsync);

    public ReactiveCommand<SplitloaderViewModel, Task> ConcatCommand { get; } =
        ReactiveCommand.Create<SplitloaderViewModel, Task>(UiTools.ConcatVideoAsync);
}