﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchService.Common.HighLight
{
    public interface Formatter
    {
        string HighlightTerm(string originalText);

    }
}
