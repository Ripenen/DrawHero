using Processes;

namespace UI
{
    public class SoundButton : ActionButton<bool>
    {
        private void Awake()
        {
            SetInteractable(!Sound.Sound.Pause);
        }

        protected override void OnClick() 
        {
            if(Open)
                AppMetricaWeb.Event("mute");
            
            SetInteractable(!Open);

            Sound.Sound.SetPause(!Open);
        }
    }
}