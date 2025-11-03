using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using RwagLab.Models.Classes;
using RwagLab.Models.Data;

namespace RwagLab.Services;

public class ScriptItemService {
    public ScriptItemService() {
        LabSpriptItems = ScriptsData.Scripts;
    }

    public ObservableCollection<LabSpriptItem> LabSpriptItems { get; init; }
}
