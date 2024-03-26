using ENTITIES.Entities.Generic;

namespace ENTITIES.Entities.Test;

public class Test : Auditable
{
    public int Test1 { get; set; }
    public File File {  get; set; }

    public List<int> Test2 { get; set; }

    public List<File> Files { get; set; }

    public string STest { get; set; }
}
