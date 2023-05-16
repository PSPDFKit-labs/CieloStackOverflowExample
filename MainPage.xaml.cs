using PSPDFKit.Document;
using PSPDFKit.UI;
using PSPDFKit.UI.ToolbarComponents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using PSPDFKit.Pdf;

namespace CieloStackOverflowExample
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            // This handler is invoked once the `PdfView` has been initialized, and then the `PdfView.Controller` can be used.
            PdfView.InitializationCompletedHandler += delegate (PdfView sender, Document args)
            {
                SetToolbarItems();

                // Now that the `PdfView` is ready, enable the button for opening a file.
                Button_OpenPDF.IsEnabled = true;
            };
        }
        
        private async void SetToolbarItems() 
        {
            var toolbarItems = PdfView.GetToolbarItems();

            var idxPan = toolbarItems.IndexOf(new PanToolbarItem());
            if (idxPan >= 0) 
            {
                toolbarItems.RemoveAt(idxPan);
            }

            var idxPrint = toolbarItems.IndexOf(new PrintToolbarItem());
            if (idxPrint >= 0) 
            {
                toolbarItems.RemoveAt(idxPrint);
            }

            var idxDocumentEditor = toolbarItems.IndexOf(new DocumentEditorToolbarItem());
            if (idxDocumentEditor >= 0) 
            {
                toolbarItems.RemoveAt(idxDocumentEditor);
            }

            await PdfView.SetToolbarItemsAsync(toolbarItems);
        }

        private async void Button_OpenPDF_Click(object sender, RoutedEventArgs e) 
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".pdf");

            var file = await picker.PickSingleFileAsync();
            if (file != null) 
            {
                var document = DocumentSource.CreateFromStorageFile(file);
                await PdfView.Controller.ShowDocumentAsync(document);
                await PdfView.Controller.SetZoomModeAsync(PSPDFKit.UI.ZoomMode.FitToViewPort);
            }
        }
    }
}
