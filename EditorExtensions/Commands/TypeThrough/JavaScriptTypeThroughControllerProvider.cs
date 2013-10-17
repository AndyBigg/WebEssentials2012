﻿using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using Microsoft.Web.Editor;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace MadsKristensen.EditorExtensions
{
    [Export(typeof(IIntellisenseControllerProvider))]
    [ContentType("JavaScript")]
    [Name("JavaScript Type Through Completion Controller")]
    [Order(Before = "Default Completion Controller")]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    internal class JavaScriptTypeThroughControllerProvider : IIntellisenseControllerProvider
    {
        public IIntellisenseController TryCreateIntellisenseController(ITextView view, IList<ITextBuffer> subjectBuffers)
        {
            if (subjectBuffers.Count > 0 && (subjectBuffers[0].ContentType.IsOfType("JavaScript")))
            {
                var completionController = ServiceManager.GetService<TypeThroughController>(subjectBuffers[0]);

                if (completionController == null)
                    completionController = new JavaScriptTypeThroughController(view, subjectBuffers);

                return completionController;
            }

            return null;
        }
    }

    internal class JavaScriptTypeThroughController : TypeThroughController
    {
        public JavaScriptTypeThroughController(ITextView textView, IList<ITextBuffer> subjectBuffers)
            : base(textView, subjectBuffers)
        {
        }

        protected override bool CanComplete(ITextBuffer textBuffer, int position)
        {
            bool result = WESettings.GetBoolean(WESettings.Keys.JavaScriptAutoCloseBraces);

            if (result)
            {
                var line = textBuffer.CurrentSnapshot.GetLineFromPosition(position);
                result = line.Start.Position + line.GetText().TrimEnd('\r', '\n', ' ', ';', ',').Length == position + 1;
            }

            return result;
        }

        protected override char GetCompletionCharacter(char typedCharacter)
        {
            switch (typedCharacter)
            {
                case '[':
                    return ']';

                case '(':
                    return ')';

                case '{':
                    return '}';
            }

            return '\0';
        }
    }
}
