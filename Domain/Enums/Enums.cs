namespace Hospisim.Domain.Enums
{

    public enum Sexo { Masculino, Feminino, Outro }
    public enum TipoSanguineo { APos, ANeg, BPos, BNeg, ABPos, ABNeg, OPos, ONeg }
    public enum EstadoCivil { Solteiro, Casado, Separado, Divorciado, Viuvo }
    public enum TipoRegistro { CRM, COREN, CRO, CRP, CRF }
    public enum Turno { Manha, Tarde, Noite }
    public enum TipoAtendimento { Emergencia, Consulta, Internacao }
    public enum StatusAtendimento { Realizado, EmAndamento, Cancelado }
    public enum StatusPrescricao { Ativa, Suspensa, Encerrada }
    public enum StatusInternacao { Ativa, AltaConcedida, Transferido, Obito }

}
