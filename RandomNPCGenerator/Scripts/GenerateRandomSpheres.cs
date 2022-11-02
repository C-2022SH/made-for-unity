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
        // 몇 개를 만들까
        [SerializeField]
        private int _npcCount;

        // 어디를 중심으로 퍼뜨릴까
        // local position
        [SerializeField]
        private Vector3 _center;

        // 양 끝점으로부터 얼마나 여백을 둘까
        // 알고리즘 특성 상,, 완벽하게 맞지 않을 수 있다
        [SerializeField]
        private float _planeHoriOffset;
        [SerializeField]
        private float _planeVertiOffset;

        // 한 줄에 최대 몇 개를 넣을 수 있다고 할까
        // 반경이다. 즉 설정한 값의 두 배가 된다
        // 가로 * 세로 = 최대 수용 개수
        [SerializeField]
        private int _horiHalfDensity;
        [SerializeField]
        private int _vertiHalfDensity;

        // 어느 정도로 격자를 유지할까
        // 클수록 격자에 가까워진다
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
                // 최대 격자 개수를 초과하면 아예 안 만든다
                // 초과하는 만큼 위에 쌓도록 해도 괜찮을 듯,, TBD
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

            // 일일이 집어 넣는다 조금 귀찮고,, 시간 복잡도가 금방 커지는 작업이다
            // 효율적으로 만드는 방법이 있을까?? TBD
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