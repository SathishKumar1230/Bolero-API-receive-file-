public class Rootobject
{
    public ConnectionmoduleMessageheader ConnectionModuleMessageHeader { get; set; }
    public Trainstructionresponses TraInstructionResponses { get; set; }
}

public class ConnectionmoduleMessageheader
{
    public ConnectionmoduleTransaction ConnectionModuleTransaction { get; set; }
}

public class ConnectionmoduleTransaction
{
    public string ConnectionModuleCompanyName { get; set; }
    public string ConnectionModuleTransactionId { get; set; }
    public string ConnectionModuleService { get; set; }
    public string ConnectionModuleSeriesTotalCount { get; set; }
    public string ConnectionModuleSeriesFile { get; set; }
    public string ConnectionModuleDate { get; set; }
}

public class Trainstructionresponses
{
    public Trainstructionresponse TraInstructionResponse { get; set; }
}

public class Trainstructionresponse
{
    public string TraInstructionId { get; set; }
    public string Instruction { get; set; }
    public string Result { get; set; }
    public Rejections Rejections { get; set; }
    public Documentid DocumentId { get; set; }
    public Parties Parties { get; set; }
}

public class Rejections
{
    public ConnectionmoduleRejection ConnectionModuleRejection { get; set; }
}

public class ConnectionmoduleRejection
{
    public string ConnectionModuleRejectionSource { get; set; }
    public string ConnectionModuleRejectionTime { get; set; }
    public string ConnectionModuleRejectionCode { get; set; }
    public string ConnectionModuleRejectionDescription { get; set; }
}

public class Documentid
{
    public Documentrid DocumentRid { get; set; }
    public string GeneralId { get; set; }
    public string Version { get; set; }
}

public class Documentrid
{
    public string ConnectionModuleRid { get; set; }
    public string ConnectionModuleCompanyName { get; set; }
}

public class Parties
{
    public Party Party { get; set; }
}

public class Party
{
    public string PartyType { get; set; }
    public Partyrid PartyRid { get; set; }
}

public class Partyrid
{
    public string ConnectionModuleRid { get; set; }
    public string ConnectionModuleCompanyName { get; set; }
}
