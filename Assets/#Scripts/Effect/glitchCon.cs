using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class glitchCon : MonoBehaviour
{
    public IE.RichFX.Glitch _glitchC;
    // Start is called before the first frame update
    void Start()
    {
        // �q���̃O���b�W�R���|�[�l���g���擾
         _glitchC = this.gameObject.GetComponent<IE.RichFX.Glitch>();
        _glitchC.block.value = 0.5f;

    }

    // Update is called once per frame
    void Update()
    {
        _glitchC.block.value = 0.5f;
    }
}
