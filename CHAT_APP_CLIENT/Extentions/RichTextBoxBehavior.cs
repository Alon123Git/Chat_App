using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Diagnostics;
using System.Windows.Data;

namespace CHAT_APP_CLIENT.Extensions
{
    public static class RichTextBoxBindingHelper
    {
        public static readonly DependencyProperty TextBoxMessageProperty =
            DependencyProperty.RegisterAttached(
                "TextBoxMessage",
                typeof(string),
                typeof(RichTextBoxBindingHelper),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextBoxMessageChanged));

        public static string GetTextBoxMessage(DependencyObject obj)
        {
            return (string)obj.GetValue(TextBoxMessageProperty);
        }

        public static void SetTextBoxMessage(DependencyObject obj, string value)
        {
            obj.SetValue(TextBoxMessageProperty, value);
        }

        private static void OnTextBoxMessageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RichTextBox richTextBox)
            {
                var newText = e.NewValue as string;

                // Ensure we don't overwrite the text unless it's necessary
                if (!string.IsNullOrEmpty(newText))
                {
                    richTextBox.Document.Blocks.Clear();
                    richTextBox.Document.Blocks.Add(new Paragraph(new Run(newText)));
                }

                // Subscribe to the TextChanged event
                richTextBox.TextChanged -= RichTextBox_TextChanged;
                richTextBox.TextChanged += RichTextBox_TextChanged;
            }
        }

        private static void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var richTextBox = sender as RichTextBox;
            if (richTextBox != null)
            {
                var textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
                var newText = textRange.Text.Trim();

                // Update the attached TextBoxMessage property
                SetTextBoxMessage(richTextBox, newText);

                // Update the binding to ensure the ViewModel gets the new value
                var binding = BindingOperations.GetBindingExpression(richTextBox, TextBoxMessageProperty);
                binding?.UpdateSource();

                Debug.WriteLine($"RichTextBox content updated: {newText}");
            }
        }
    }
}
