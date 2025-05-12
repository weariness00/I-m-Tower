namespace Util.Condition
{
    public struct ConditionUtil
    {
        public delegate bool ConditionAction(); 
        public event ConditionAction onConditionEvent; // 조건을 확인하는 델리게이트

        public bool IsCondition => CheckCondition();
        
        // 모든 조건 확인
        private bool CheckCondition()
        {
            if (onConditionEvent != null)
            {
                foreach (var del in onConditionEvent.GetInvocationList())
                {
                    if (del is ConditionAction condition && !condition())
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}