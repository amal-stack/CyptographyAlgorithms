using CyptographyAlgorithms.Console;

ProgramName programName = ProgramName.KnapsackCryptosystem;


Dictionary<ProgramName, Type> programTypes = new()
{
    { ProgramName.None, typeof(NullRunner) },
    { ProgramName.ShiftCipher, typeof(ShiftCipherRunner) },
    { ProgramName.MonoalphabeticSubstitutionCipher, typeof(MonoalphabeticSubtitutionCipherRunner) },
    { ProgramName.Spn, typeof(SpnRunner) },
    { ProgramName.Lfsr, typeof(LfsrRunner) },
    { ProgramName.Aes, typeof(AesRunner) },
    { ProgramName.AesKeySchedule, typeof(AesKeyScheduleRunner) },
    { ProgramName.KnapsackCryptosystem, typeof(KnapsackRunner) },
    { ProgramName.RsaSignatureScheme, typeof(RsaSignatureSchemeRunner) },
};




if (programTypes.TryGetValue(programName, out Type? type))
{
    type.GetMethod("Run")?.Invoke(null, Array.Empty<object>());
}



enum ProgramName
{
    None = 0,
    ShiftCipher = 1,
    MonoalphabeticSubstitutionCipher = 2,
    Spn = 3,
    Lfsr = 4,
    Des = 5,
    Aes = 6,
    AesKeySchedule = 7,
    KnapsackCryptosystem = 8,
    RsaSignatureScheme = 9
}
