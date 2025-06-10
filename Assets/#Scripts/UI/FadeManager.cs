using UnityEngine;

public class FadeManager : MonoBehaviour
{
    [SerializeField]
    FadeAnimation m_fadeInAnim;
    [SerializeField]
    FadeAnimation m_fadeOutAnim;

    #region プロパティ
    public bool FadeInComplete => m_fadeInAnim.GetEndAnimationFlagOnce();

    public bool FadeOutComplete => m_fadeOutAnim.GetEndAnimationFlagOnce();

	#endregion

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Awake()
    {
        m_fadeInAnim.gameObject.SetActive(true);
        m_fadeOutAnim.gameObject.SetActive(true);
        m_fadeInAnim.StartAnimation = false;
        m_fadeOutAnim.StartAnimation = false;
    }

    public void PlayFadeIn()
    {
		m_fadeInAnim.StartAnimation = true;
	}

    public void PlayFadeOut()
    {
        m_fadeOutAnim.StartAnimation = true;
    }
}
