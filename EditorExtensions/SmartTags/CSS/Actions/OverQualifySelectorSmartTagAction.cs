﻿using Microsoft.CSS.Core;
using Microsoft.VisualStudio.Text;
using System;
using System.Windows.Media.Imaging;

namespace MadsKristensen.EditorExtensions
{
    internal class OverQualifySelectorSmartTagAction : CssSmartTagActionBase
    {
        private readonly Selector _selector;
        private readonly ITrackingSpan _span;
        private readonly int _index;

        public OverQualifySelectorSmartTagAction(Selector sel, ITrackingSpan span, int index)
        {
            _selector = sel;
            _span = span;
            _index = index;

            if (Icon == null)
            {
                Icon = BitmapFrame.Create(new Uri("pack://application:,,,/WebEssentials2012;component/Resources/warning.png", UriKind.RelativeOrAbsolute));
            }
        }

        public override string DisplayText
        {
            get { return Resources.OverQualifiedSmartTagActionName  ; }
        }

        public override void Invoke()
        {
            Span ruleSpan = new Span(_selector.Start, _index);

            EditorExtensionsPackage.DTE.UndoContext.Open(DisplayText);
            _span.TextBuffer.Delete(ruleSpan);
            EditorExtensionsPackage.DTE.UndoContext.Close();
        }
    }

}