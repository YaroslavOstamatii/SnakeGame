﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public class Snake
    {
       
        public Snake(int initialX, int initialY,ConsoleColor headColor,ConsoleColor bodyColor,int bodyLenght=3)
        {
            _headColor = headColor;
            _bodyColor = bodyColor;
            Head = new Pixel(initialX, initialY, _headColor);
            Head.Draw();
            for (int i = bodyLenght; i >0; i--)
            {
                Body.Enqueue(new Pixel(Head.X - (i*2), initialY, _bodyColor));
                
            }
            Draw();

        }
        public ConsoleColor _headColor { get; }
        public ConsoleColor _bodyColor { get; }
        public Pixel Head { get; private set; }
        public Queue<Pixel> Body { get; }=new Queue<Pixel>();
        public void Move(Direction direction, bool eat = false)
        {
            Clear();
            Body.Enqueue(new Pixel(Head.X, Head.Y, _bodyColor));
            if (!eat)
                Body.Dequeue();
            Head = direction switch
            {
                Direction.Right => new Pixel(Head.X + 2, Head.Y, _headColor),
                Direction.Left =>  new Pixel(Head.X - 2, Head.Y, _headColor),
                Direction.Up =>    new Pixel(Head.X, Head.Y - 1, _headColor),
                Direction.Down =>  new Pixel(Head.X, Head.Y + 1, _headColor),
                _ => Head
            };
            Draw();
        }
        public void Draw()
        {
            Head.Draw();
            foreach (Pixel pixel in Body )
            {
                pixel.Draw();   
            }
        }
        public void Clear()
        {
            Head.Clear();
            foreach (Pixel pixel in Body)
            {
                pixel.Clear();
            }
        }
    }
}
