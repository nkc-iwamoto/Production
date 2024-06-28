using UnityEngine;

public class Switch : MonoBehaviour, IRayReceiver
{
    // �S�[���o���郌�[�U�[�̐F
    public enum COLOR
    {
        White,
        Red,
        Blue,
        Green,
        Purple,
        Yellow,
        Cyan,
    }

    [SerializeField, Header("�X�C�b�`�̌��镔��")]
    GameObject[] switchObj;

    [SerializeField, Header("�X�C�b�`�̃J�E���g�A�b�v�̃X�N���v�g")]
    SwitchCountUp switchCountUp;

    [SerializeField, Header("���̋���")]
    int intensity;

    [SerializeField, Header("�S�[���ł��郌�[�U�[�̐F")]
    COLOR enumGoalColor;

    [SerializeField, Header("���[�U�[�̐F�f�[�^")]
    LaserColorData laserColorData;

    // �擾�������[�U�[�̐F���
    int laserColorNum;

    // �S�[���ł���F���
    int enumGoalColorNum;

    // 
    public int GetGoalColor { get { return (int)enumGoalColor; } }

    // �X�C�b�`�̏�����(�����Ă��Ȃ���)����F
    private Color initColor;

    //�@�X�C�b�`�������Ă��邩
    bool isOnLight;


    public bool getIsOnLight => isOnLight;

    private void Start()
    {
        // �F�������ɏ���������F���擾
        initColor = laserColorData.colors[(int)enumGoalColor];
        // �X�C�b�`�̏�����
        switchCountUp.Init(switchObj, initColor);
        // �����Ă��Ȃ�
        isOnLight = false;
    }

    private void Update()
    {
        // �����Ă��Ȃ��Ƃ�
        if (!isOnLight) { return; }

        // ���[�U�[�̐F�ƃS�[���ł���F��������
        if (laserColorNum != enumGoalColorNum)
        {
            // �X�C�b�`�̏�����
            RayExit();
            return;
        }
    }

    // ���[�U�[��������Ȃ��Ȃ�����
    public void RayExit()
    {
        // �X�C�b�`�̏�����
        switchCountUp.Init(switchObj, initColor);
        // �����Ă��Ȃ�
        isOnLight = false;
    }

    // ���[�U�[���������Ă���Ƃ�
    public void RayStay(Laser laser, int f_count, int maxConvertCount)
    {
        // ���[�U�[�̐F�����擾
        laserColorNum = laser.ColorNum;

        // �S�[���ł���F���擾
        enumGoalColorNum = (int)enumGoalColor;

        // ���[�U�[�̐F�ƃS�[���ł���F��������
        if (laserColorNum != enumGoalColorNum) { return; }

        // ���[�U�[�̐F�������ɐF���擾
        Color color = laserColorData.colors[laser.ColorNum];

        // �X�C�b�`�����炷
        isOnLight = switchCountUp.IsOnSwitch(switchObj, color, intensity);
    }
}
