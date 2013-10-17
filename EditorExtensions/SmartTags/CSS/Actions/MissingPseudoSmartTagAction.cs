﻿using Microsoft.CSS.Core;
using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace MadsKristensen.EditorExtensions
{
    internal class MissingPseudoSmartTagAction : CssSmartTagActionBase
    {
        private readonly ITrackingSpan _span;
        private readonly ParseItem _pseudo;
        private readonly Selector _selector;
        private readonly IEnumerable<string> _missingPseudos;

        public MissingPseudoSmartTagAction(ITrackingSpan span, Selector selector, ParseItem pseudo, IEnumerable<string> missingPseudos)
        {
            _span = span;
            _selector = selector;
            _pseudo = pseudo;
            _missingPseudos = missingPseudos;

            if (Icon == null)
            {
                Icon = BitmapFrame.Create(new Uri("pack://application:,,,/WebEssentials2012;component/Resources/warning.png", UriKind.RelativeOrAbsolute));
            }
        }

        public override string DisplayText
        {
            get { return string.Format("Add missing pseudo ({0})", string.Join(", ", _missingPseudos)); }
        }

        public override void Invoke()
        {
            StringBuilder sb = new StringBuilder();
         
            foreach (var entry in _missingPseudos)
            {
                string text = _selector.Text.Replace(_pseudo.Text, entry).Trim(',');
                sb.Append(text + "," + Environment.NewLine);
            }
            
            EditorExtensionsPackage.DTE.UndoContext.Open(DisplayText);
            _span.TextBuffer.Insert(_selector.Start, sb.ToString());
            EditorExtensionsPackage.ExecuteCommand("Edit.FormatSelection");
            EditorExtensionsPackage.DTE.UndoContext.Close();
        }
    }
}
