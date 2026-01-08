using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace RwagLab.Models.Classes;

public class ScriptGroup {
    public required string GroupID {  get; set; }

    public required string Name {  get; set; }

    public required IconElement Icon {  get; set; }
}
