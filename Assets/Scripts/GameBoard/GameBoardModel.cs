using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class GameBoardModel
{
    private int _size = 4;
    private int?[,] _matrix;
    private Vector2 _emptyCell;

    public int Size => _size;
    public int?[,] Matrix => _matrix;


    public ReactiveCommand<TagPresentor> OnTagMoved { get; } = new();
    public ReactiveCommand OnWin { get; } = new();
    

    public void SpawnPrepare()
    {
        _emptyCell = new Vector2(_size - 1, _size - 1);
    }

    public bool CheckWinCondition()
    {
        int expected = 1;

        for (int y = 0; y < _size; y++)
        {
            for (int x = 0; x < _size; x++)
            {
                if (x == _size - 1 && y == _size - 1)
                {
                    if (_matrix[x, y].HasValue)
                    {
                        return false;
                    }
                }
                else
                {
                    if (!_matrix[x, y].HasValue || _matrix[x, y].Value != expected)
                    {
                        return false;
                    }
                    expected++;
                }
            }
        }

        return true;
    }

    public async UniTaskVoid TryMove(TagPresentor presentor)
    {
        var tagModel = presentor.Model;
        
        if (Mathf.Abs(tagModel.X.Value - _emptyCell.x) +
            Mathf.Abs(tagModel.Y.Value - _emptyCell.y) != 1)
            return;

        int oldX = tagModel.X.Value, oldY = tagModel.Y.Value;
        int newX = (int)_emptyCell.x, newY = (int)_emptyCell.y;

        _matrix[oldX, oldY] = null;
        _matrix[newX, newY] = tagModel.Number.Value;
        _emptyCell = new Vector2(oldX, oldY);

        Vector3 targetPos = GetLocalPosition(newX, newY, tagModel.GetCellSize());

        tagModel.X.Value = newX;
        tagModel.Y.Value = newY;

        OnTagMoved.Execute(presentor);
      

        if (CheckWinCondition())
        {
            OnWin.Execute();
        }
    }

    public UniTask GenerateTagsMatrix()
    {
        _matrix = new int?[_size, _size];

        do
        {
            List<int> numbers = Enumerable.Range(1, _size * _size - 1).ToList();
            for (int y = 0; y < _size; y++)
            {
                for (int x = 0; x < _size; x++)
                {
                    if (x == _size - 1 && y == _size - 1) continue;

                    int randIndex = UnityEngine.Random.Range(0, numbers.Count);
                    _matrix[x, y] = numbers[randIndex];
                    numbers.RemoveAt(randIndex);
                }
            }
        } while (!IsSolvable(_matrix));
        
        return UniTask.CompletedTask;
    }

    private int[] ConvertMatrixToFlatArray(int?[,] matrix)
    {
        int size = matrix.GetLength(0);
        int[] result = new int[size * size - 1];
        int index = 0;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                if (matrix[x, y].HasValue)
                {
                    result[index++] = matrix[x, y].Value;
                }
            }
        }
        return result;
    }

    private bool IsSolvable(int?[,] matrix)
    {
        int size = matrix.GetLength(0);
        int[] flat = ConvertMatrixToFlatArray(matrix);

        int countInversions = 0;
        for (int i = 0; i < flat.Length - 1; i++)
        {
            for (int j = i + 1; j < flat.Length; j++)
            {
                if (flat[i] > flat[j]) countInversions++;
            }
        }

        if (size % 2 == 1)
        {
            return countInversions % 2 == 0;
        }
        else
        {
            int emptyRow = -1;
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    if (!matrix[x, y].HasValue)
                    {
                        emptyRow = size - y;
                        break;
                    }
                }
                if (emptyRow != -1) break;
            }

            if (emptyRow % 2 == 0)
                return countInversions % 2 == 1;
            else
                return countInversions % 2 == 0;
        }
    }

    public Vector2 GetLocalPosition(int x, int y, float tagSize)
    {
        float offset = (_size - 1) * tagSize / 2f;
        return new Vector2((x * tagSize) - offset, -((y * tagSize) - offset));
    }
}
