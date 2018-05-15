using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Joysticks
{
    public enum JoystickAxis
    {
        HORIZONTAL,
        VERTICAL,
        THUMB_HORIZOINTAL,
        THUMB_VERTICAL,
        TRIGGER,
        DPAD_X,
        DPAD_Y
    }

    public enum JoystickButton
    {
        A,
        B,
        X,
        Y,
        BUMPER_L,
        BUMPER_R,
        START,
        SELECT,
        CLICK_STICK_L,
        CLICK_STICK_R
    }

    public class JoystickManager
    {
        private const string JOYSTICK = "Joystick";

        // AXIS ON WINDOWS
        private string horizontal;
        private string vertical;
        private string thumbHorizontal;
        private string thumbVertical;
        private string dpadX;
        private string dpadY;
        private string trigger;

        // BUTTONS ON WINDOWS
        private string buttonA;
        private string buttonB;
        private string buttonX;
        private string buttonY;
        private string bumperL;
        private string bumperR;
        private string start;
        private string select;
        private string clickStickL;
        private string clickStickR;

        // MISSING CONTROLLS FOR THE PLATTFORMS, BECAUSE OF INCONSITENCY

        public JoystickManager(float joystickNumber)
        {
            // AXIS
            horizontal = JOYSTICK + joystickNumber + "Horizontal";
            vertical = JOYSTICK + joystickNumber + "Vertical";
            thumbHorizontal = JOYSTICK + joystickNumber + "ThumbHorizontal";
            thumbVertical = JOYSTICK + joystickNumber + "ThumbVertical";
            dpadX = JOYSTICK + joystickNumber + "DpadX";
            dpadY = JOYSTICK + joystickNumber + "DpadY";
            trigger = JOYSTICK + joystickNumber + "Trigger";

            // BUTTONS
            buttonA = JOYSTICK + joystickNumber + "ButtonA";
            buttonB = JOYSTICK + joystickNumber + "ButtonB";
            buttonX = JOYSTICK + joystickNumber + "ButtonX";
            buttonY = JOYSTICK + joystickNumber + "ButtonY";
            bumperL = JOYSTICK + joystickNumber + "BumperL";
            bumperR = JOYSTICK + joystickNumber + "BumperR";
            start = JOYSTICK + joystickNumber + "ButtonStart";
            select = JOYSTICK + joystickNumber + "ButtonSelect";
            clickStickL = JOYSTICK + joystickNumber + "ButtonStickL";
            clickStickR = JOYSTICK + joystickNumber + "ButtonStickR";

            // MAC AND LINUX HAVE TO BE DONE
            // MAYBE WRITE AN INTERFACE FOR THE BUTTONS
        }

        public float GetAxis(JoystickAxis axis)
        {
            switch (axis)
            {
                case JoystickAxis.HORIZONTAL:
                    return Input.GetAxis(horizontal);

                case JoystickAxis.VERTICAL:
                    return Input.GetAxis(vertical);

                case JoystickAxis.THUMB_HORIZOINTAL:
                    return Input.GetAxis(thumbHorizontal);

                case JoystickAxis.THUMB_VERTICAL:
                    return Input.GetAxis(thumbVertical);

                case JoystickAxis.TRIGGER:
                    return Input.GetAxis(trigger);

                case JoystickAxis.DPAD_X:
                    return Input.GetAxis(dpadX);

                case JoystickAxis.DPAD_Y:
                    return Input.GetAxis(dpadY);

                default:
                    return 0f;
            }
        }

        public bool CheckButton(JoystickButton button, Func<string, bool> func)
        {
            switch (button)
            {
                case JoystickButton.A:
                    return func(buttonA);

                case JoystickButton.B:
                    return func(buttonB);

                case JoystickButton.X:
                    return func(buttonX);

                case JoystickButton.Y:
                    return func(buttonY);

                case JoystickButton.BUMPER_L:
                    return func(bumperL);

                case JoystickButton.BUMPER_R:
                    return func(bumperR);

                case JoystickButton.START:
                    return func(start);

                case JoystickButton.SELECT:
                    return func(select);

                case JoystickButton.CLICK_STICK_L:
                    return func(clickStickL);

                case JoystickButton.CLICK_STICK_R:
                    return func(clickStickR);

                default:
                    return false;
            }
        }
    }
}