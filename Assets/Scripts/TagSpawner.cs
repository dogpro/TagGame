using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class TagSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _tag;
    [SerializeField] private Transform _tagParent;

    [Space]
    [SerializeField] private int _tagCount = 15;
    [SerializeField] private int _fieldSize = 4;
    [SerializeField] private float _tagSpacing = 10f;
    [SerializeField] private float _size = 100f;
    [SerializeField] private float _moveDuration = 0.2f;

    private Tag[,] _tagsMatrix;
    private Vector2 _emptyCell;

    [Inject] private IObjectResolver _container;
    [Inject] private GameStats _gameStats;
    [Inject] private SceneLoader _sceneLoader;

    public void SpawnTags()
    {
        _tagsMatrix = new Tag[_fieldSize, _fieldSize];

        int[,] matrix = GenerateTagsMatrix();

        _emptyCell = new Vector2(_fieldSize - 1, _fieldSize - 1);

        for (int y = 0; y < _fieldSize; y++)
        {
            for (int x = 0; x < _fieldSize; x++)
            {
                if (matrix[x, y] == 0) continue;

                var tagObj = Instantiate(_tag, _tagParent);
                _container.InjectGameObject(tagObj);

                tagObj.transform.localPosition = GetLocalPosition(x, y);

                var tagComponent = tagObj.GetComponent<Tag>();
                tagComponent.Number.Value = matrix[x, y];
                tagComponent.TagSize.Value = _size;
                tagComponent.X.Value = x;
                tagComponent.Y.Value = y;
                tagComponent.Init();

                _tagsMatrix[x, y] = tagComponent;
            }
        }
    }    

    public async UniTaskVoid TryMove(Tag tag)
    {
        if (Mathf.Abs(tag.X.Value - _emptyCell.x) + Mathf.Abs(tag.Y.Value - _emptyCell.y) != 1)
            return;

        int oldX = tag.X.Value, oldY = tag.Y.Value;
        int newX = (int)_emptyCell.x, newY = (int)_emptyCell.y;

        _tagsMatrix[oldX, oldY] = null;
        _tagsMatrix[newX, newY] = tag;
        _emptyCell = new Vector2(oldX, oldY);

        Vector3 targetPos = GetLocalPosition(newX, newY);

        tag.X.Value = newX;
        tag.Y.Value = newY;

        await tag.AnimateMove(targetPos, _moveDuration);

        _gameStats.RegisterStep();

        CheckWinCondition();
    }

    private Vector2 GetLocalPosition(int x, int y)
    {
        float tagSize = _size + _tagSpacing;
        float offsetX = (_fieldSize - 1) * tagSize / 2f;
        float offsetY = (_fieldSize - 1) * tagSize / 2f;

        return new Vector2(
            (x * tagSize) - offsetX,
            -((y * tagSize) - offsetY)
        );
    }

    private int[,] GenerateTagsMatrix()
    {
        int[,] matrix = new int[_fieldSize, _fieldSize];

        do
        {
            List<int> numbers = Enumerable.Range(1, 15).ToList();
            for (int y = 0; y < _fieldSize; y++)
            {
                for (int x = 0; x < _fieldSize; x++)
                {
                    if (x == _fieldSize - 1 && y == _fieldSize - 1) continue;

                    int randIndex = UnityEngine.Random.Range(0, numbers.Count);
                    matrix[x, y] = numbers[randIndex];
                    numbers.RemoveAt(randIndex);
                }
            }
        } while (IsSolvable(matrix.Cast<int>().ToArray()));

        return matrix;
    }

    private bool IsSolvable(int[] array)
    {
        int countInversions = 0;

        for (int i = 0; i < array.Length; i++)
        {
            for (int j = 0; j < i; j++)
            {
                if (array[j] > array[i])
                {
                    countInversions++;
                }
            }
        }

        return countInversions % 2 == 0;
    }

    private async void CheckWinCondition()
    {
        int expectedNumber = 1;

        for (int y = 0; y < _fieldSize; y++)
        {
            for (int x = 0; x < _fieldSize; x++)
            {
                if (x == _fieldSize - 1 && y == _fieldSize - 1)
                {
                    if (_tagsMatrix[x, y] != null)
                        return;
                    continue;
                }

                var tag = _tagsMatrix[x, y];

                if (tag == null || tag.Number.Value != expectedNumber)
                    return;

                expectedNumber++;
            }
        }

        _gameStats.StopTracking();
        await _sceneLoader.ShowWinPopupAsync(this.transform);
       
    }

    [ContextMenu("Solved")]
    public async void ArrangeTagsInOrder()
    {
        _gameStats.StopTracking();
        await _sceneLoader.ShowWinPopupAsync(this.transform);
    }
}

