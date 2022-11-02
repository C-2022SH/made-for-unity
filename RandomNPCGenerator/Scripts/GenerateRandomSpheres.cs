using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.RandomNPC
{
    public class GenerateRandomSpheres : MonoBehaviour
    {
        [Header("objects")]
        [SerializeField]
        private Transform tr_plane;
        [SerializeField]
        private GameObject _spherePrefab;

        [Header("variables")]
        // �� ���� �����
        [SerializeField]
        private int _npcCount;

        // ��� �߽����� �۶߸���
        // local position
        [SerializeField]
        private Vector3 _center;

        // �� �������κ��� �󸶳� ������ �ѱ�
        // �˰��� Ư�� ��,, �Ϻ��ϰ� ���� ���� �� �ִ�
        [SerializeField]
        private float _planeHoriOffset;
        [SerializeField]
        private float _planeVertiOffset;

        // �� �ٿ� �ִ� �� ���� ���� �� �ִٰ� �ұ�
        // �ݰ��̴�. �� ������ ���� �� �谡 �ȴ�
        // ���� * ���� = �ִ� ���� ����
        [SerializeField]
        private int _horiHalfDensity;
        [SerializeField]
        private int _vertiHalfDensity;

        // ��� ������ ���ڸ� �����ұ�
        // Ŭ���� ���ڿ� ���������
        [SerializeField]
        private float _randomRate;

        private float[] _slotSize = new float[2];
        private List<int[]> _slots = new List<int[]>();

        public void Start()
        {
            _slotSize[0] = (1f - _planeHoriOffset) / (float)(_horiHalfDensity * 2);
            _slotSize[1] = (1f - _planeVertiOffset) / (float)(_vertiHalfDensity * 2);

            populateNPCSpheres();
        }

        private void populateNPCSpheres()
        {
            resetSlots();

            if(_npcCount > _slots.Count)
            {
                // �ִ� ���� ������ �ʰ��ϸ� �ƿ� �� �����
                // �ʰ��ϴ� ��ŭ ���� �׵��� �ص� ������ ��,, TBD
                Debug.LogError("trying to add more npcs than the density allows");
                return;
            }

            for (int i = 0; i < _npcCount; ++i)
            {
                var sphere = Instantiate(_spherePrefab, tr_plane).GetComponent<NPCSphere>();
                sphere.setup(getRandomPosition());
            }
        }

        private void resetSlots()
        {
            _slots.Clear();

            // ������ ���� �ִ´� ���� ������,, �ð� ���⵵�� �ݹ� Ŀ���� �۾��̴�
            // ȿ�������� ����� ����� ������?? TBD
            for (int hori = -_horiHalfDensity; hori < _horiHalfDensity; ++hori)
            {
                for (int verti = -_vertiHalfDensity; verti < _vertiHalfDensity; ++verti)
                {
                    int[] slot = { hori, verti };
                    _slots.Add(slot);
                }
            }
        }

        private int[] getRandomSlot()
        {
            int index = Random.Range(0, _slots.Count);
            int[] slot = _slots[index];
            _slots.RemoveAt(index);
            return slot;
        }

        private Vector3 getDelta()
        {
            int[] slot = getRandomSlot();
            Vector3 origin = new Vector3((float)slot[0] * _slotSize[0], 0, (float)slot[1] * _slotSize[1]);
            float horiRand = Random.Range(-_slotSize[0] / _randomRate, _slotSize[0] / _randomRate);
            float vertiRand = Random.Range(-_slotSize[1] / _randomRate, _slotSize[1] / _randomRate);

            return new Vector3(origin.x + horiRand, 0, origin.z + vertiRand);
        }

        private Vector3 getRandomPosition()
        {
            Vector3 delta = getDelta();
            return _center + delta;
        }
    }
}