using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

public enum CurrencySize
{
    [Description("")]
    Normal = 1, // 0 - 999

    [Description("K")]
    K = 2, // 1 000

    [Description("M")]
    M = 3, // 1 000 000

    [Description("B")]
    B = 4, // 1 000 000 000

    [Description("T")]
    T = 5, // 1 000 000 000 000 

    [Description("q")]
    q = 6, // 1 000 000 000 000 000

    [Description("Q")]
    Q = 7,  // 1 000 000 000 000 000 000

    [Description("s")]
    s = 7,  // 1 000 000 000 000 000 000 000

    [Description("S")]
    S = 7,  // 1 000 000 000 000 000 000 000 000

    [Description("O")]
    O = 7,  // 1 000 000 000 000 000 000 000 000 000

    [Description("N")]
    N = 7,  // 1 000 000 000 000 000 000 000 000 000 000

    [Description("d")]
    d = 7,  // 1 000 000 000 000 000 000 000 000 000 000 000

    [Description("U")]
    U = 7,  // 1 000 000 000 000 000 000 000 000 000 000 000 000

    [Description("D")]
    D = 7,  // 1 000 000 000 000 000 000 000 000 000 000 000 000 000
}
