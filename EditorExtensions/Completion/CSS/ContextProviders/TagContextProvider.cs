﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Microsoft.CSS.Core;
using Microsoft.CSS.Editor;
using Microsoft.VisualStudio.Utilities;

namespace MadsKristensen.EditorExtensions
{
    [Export(typeof(ICssCompletionContextProvider))]
    [Name("TagCompletionContextProvider")]
    internal class TagCompletionContextProvider : ICssCompletionContextProvider
    {
        public TagCompletionContextProvider()
        {
        }

        public IEnumerable<Type> ItemTypes
        {
            get
            {
                return new Type[] { typeof(ItemName), };
            }
        }

        public CssCompletionContext GetCompletionContext(ParseItem item, int position)
        {
            return new CssCompletionContext((CssCompletionContextType)601, item.Start, item.Length, null);
        }
    }
}
