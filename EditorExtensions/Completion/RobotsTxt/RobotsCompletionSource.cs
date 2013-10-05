﻿using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Utilities;
using Microsoft.Web.Editor;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Media;
using Intel = Microsoft.VisualStudio.Language.Intellisense;

namespace MadsKristensen.EditorExtensions
{
    [Export(typeof(ICompletionSourceProvider))]
    [ContentType("plaintext")]
    [Name("RobotsTxtCompletion")]
    class RobotsTxtCompletionSourceProvider : ICompletionSourceProvider
    {
        [Import]
        private IGlyphService _glyphService = null;

        public ICompletionSource TryCreateCompletionSource(ITextBuffer textBuffer)
        {
            return new RobotsTxtCompletionSource(textBuffer, _glyphService);
        }
    }

    class RobotsTxtCompletionSource : ICompletionSource
    {
        private ITextBuffer _buffer;
        private bool _disposed = false;
        private static ImageSource _glyph;

        public RobotsTxtCompletionSource(ITextBuffer buffer, IGlyphService glyphService)
        {
            _buffer = buffer;
            _glyph = glyphService.GetGlyph(StandardGlyphGroup.GlyphGroupVariable, StandardGlyphItem.GlyphItemPublic);
        }

        public void AugmentCompletionSession(ICompletionSession session, IList<CompletionSet> completionSets)
        {
            if (_disposed)
                return;

            List<Intel.Completion> completions = new List<Intel.Completion>();
            foreach (string item in RobotsTxtClassifier._valid)
            {
                completions.Add(new Intel.Completion(item, item, null, _glyph, item));
            }
            
            ITextSnapshot snapshot = _buffer.CurrentSnapshot;
            var triggerPoint = (SnapshotPoint)session.GetTriggerPoint(snapshot);

            if (triggerPoint == null)
                return;

            var line = triggerPoint.GetContainingLine();
            string text = line.GetText();
            int index = text.IndexOf(':');
            int hash = text.IndexOf('#');
            SnapshotPoint start = triggerPoint;

            if (hash > -1 && hash < triggerPoint.Position || (index > -1 && (start - line.Start.Position) > index))
                return;

            while (start > line.Start && !char.IsWhiteSpace((start - 1).GetChar()))
            {
                start -= 1;
            }

            var applicableTo = snapshot.CreateTrackingSpan(new SnapshotSpan(start, triggerPoint), SpanTrackingMode.EdgeInclusive);

            completionSets.Add(new CompletionSet("All", "All", applicableTo, completions, Enumerable.Empty<Intel.Completion>()));
        }

        public void Dispose()
        {
            _disposed = true;
        }
    }
}