namespace ArcaneOdyssey.GlobalTypes
{
	public class AODebuff : GlobalBuff
	{
		public override void ModifyBuffText(int type, ref string buffName, ref string tip, ref int rare)
		{
			buffName = buffName.Replace("Imbue", "Gel");
		}
	}
}
