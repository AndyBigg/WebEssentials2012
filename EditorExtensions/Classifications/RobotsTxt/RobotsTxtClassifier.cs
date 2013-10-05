﻿using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using System;
using System.Collections.Generic;

namespace MadsKristensen.EditorExtensions
{
    public class RobotsTxtClassifier : IClassifier
    {
        private IClassificationType _keyword, _comment;
        private bool _isRobotsTxt = false;
        private TextType _textType;
        public static HashSet<string> _valid = new HashSet<string>() { "user-agent", "disallow", "sitemap", "crawl-delay", "host" };

        public RobotsTxtClassifier(IClassificationTypeRegistryService registry)
        {
            _keyword = registry.GetClassificationType(RobotsTxtClassificationTypes.RobotsTxtKeyword);
            _comment = registry.GetClassificationType(RobotsTxtClassificationTypes.RobotsTxtComment);
        }

        public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
        {
            IList<ClassificationSpan> list = new List<ClassificationSpan>();
            if (!_isRobotsTxt)
                return list;

            string text = span.GetText();
            int index = text.IndexOf("#");

            if (index > -1)
            {
                var result = new SnapshotSpan(span.Snapshot, span.Start + index, text.Length - index);
                list.Add(new ClassificationSpan(result, _comment));
            }

            if (_textType != TextType.Robots)
                return list;

            if (index == -1 || index > 0)
            {
                string[] args = text.Split(':');

                if (args.Length >= 2 && _valid.Contains(args[0].Trim().ToLowerInvariant()))
                {
                    var result = new SnapshotSpan(span.Snapshot, span.Start, args[0].Length);
                    list.Add(new ClassificationSpan(result, _keyword));
                }
            }

            return list;
        }

        public void RaiseClassificationChanged(SnapshotSpan span, TextType type)
        {
            _isRobotsTxt = true;
            _textType = type;
            var handler = this.ClassificationChanged;

            if (handler != null)
                handler(this, new ClassificationChangedEventArgs(span));
        }

        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;
    }
}
