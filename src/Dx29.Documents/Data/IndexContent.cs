using System;
using System.Collections.Generic;

namespace Dx29.Documents.Data
{
    public class IndexContent : Dictionary<string, DocItem>
    {
    }

    public class DocItem : Dictionary<string, DocItemLan>
    {
    }

    public class DocItemLan : Dictionary<string, DocItemLanVer>
    {
    }

    public class DocItemLanVer : Dictionary<string, string>
    {
    }
}
