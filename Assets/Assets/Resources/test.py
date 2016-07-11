#!/usr/bin/env python

import clr
clr.AddReferenceByPartialName('UnityEngine')
import UnityEngine


def print_message():
    UnityEngine.Debug.Log('Test message from Python!')


print_message()