import {
    TextArea,
    Input,
    NotificationBarAlert
} from '@kentico/xperience-admin-components';
import React, { useEffect, useState } from 'react';
import { usePageCommand } from '@kentico/xperience-admin-base';
import { CustomNotification } from '../custom-notification/CustomNotification';

export interface IRegistrationItem {
    first_name: string;
    last_name: string;
    email: string;
    apiKey: string;
}

export const AIUNRegistrationLayoutTemplate = (props: any) => {
    const registrationItem: IRegistrationItem | null = props?.registrationItem ?? null;
    const isRegistrationExist: boolean = props.isRegistrationExist;

    const [formData, setFormData] = useState<IRegistrationItem | null>(registrationItem);
    const [errors, setErrors] = useState<{ [K in keyof IRegistrationItem]?: string }>({});
    const [message, setMessage] = useState<string | null>(null);
    const [isSuccess, setIsSuccess] = useState<boolean>(true);
    const [showApiKey, setShowApiKey] = useState<boolean>(isRegistrationExist);
    const [isSaved, setIsSaved] = useState<boolean>(isRegistrationExist);

    useEffect(() => {
        if (registrationItem) {
            setFormData(registrationItem);
        }
    }, [registrationItem]);

    if (formData === null || formData?.email === '') {
        return (
            <div className="registration-page-wrapper">
                <div className="size-m___exMBL indexes-text">Register with AIUN</div>
                <div className="mt-24">
                    <NotificationBarAlert>
                        Only Global Administrator has access to this page.
                    </NotificationBarAlert>
                </div>
            </div>
        );
    }

    const validateField = (field: keyof IRegistrationItem, value: string): string => {
        if (!value.trim()) {
            return `${field.charAt(0).toUpperCase() + field.slice(1).replace('_',' ')} is required.`;
        }

        if (field === 'email') {
            const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            if (!emailRegex.test(value)) return 'Invalid email format.';
            if (value.toLowerCase().endsWith('.local')) return '.local emails are not allowed.';
        }

        return '';
    };

    const validateAll = (): boolean => {
        const newErrors: typeof errors = {};
        (['first_name', 'last_name', 'email'] as (keyof IRegistrationItem)[]).forEach((field) => {
            const value = formData?.[field] || '';
            const error = validateField(field, value);
            if (error) newErrors[field] = error;
        });

        if (isSaved && !formData?.apiKey.trim()) {
            newErrors.apiKey = 'Please enter the API key sent to your email.';
        }

        setErrors(newErrors);
        return Object.keys(newErrors).length === 0;
    };

    const handleChange = (field: keyof IRegistrationItem, value: string) => {
        setFormData({ ...formData, [field]: value });
        setMessage(null);
    };

    const handleBlur = (field: keyof IRegistrationItem) => {
        setMessage(null);
        const value = formData?.[field] || '';
        const error = validateField(field, value);
        setErrors((prev) => ({ ...prev, [field]: error }));
    };

    const { execute: submitCommand } = usePageCommand<{ success: boolean; message: string }, IRegistrationItem>(
        'StoreOrUpdateAsync',
        {
            after: (response: any) => {
                if (response.success) {
                    setIsSuccess(true);
                    setMessage(response.message || 'Data saved successfully.');
                    setShowApiKey(true);
                    setIsSaved(true);
                } else {
                    setIsSuccess(false);
                    setMessage(response.message || 'Something went wrong.');
                }
            },
        }
    );

    const handleSubmit = async () => {
        setMessage(null);
        if (!validateAll()) {
            setIsSuccess(false);
            return;
        }

        try {
            submitCommand(formData);
        } catch {
            setIsSuccess(false);
            setMessage('Unexpected error occurred.');
        }
    };

    return (
        <div className="registration-page-wrapper">
            <div className="size-m___exMBL indexes-text">Register with AIUN</div>

            <div className="registration-page-text-wrapper">
                <div className="stack___kfkMm">
                    <div>
                        <div>
                            <div className="label-wrapper___AcszK">
                                <div>
                                    <label className="label___WET63" aria-disabled="false">
                                        <span className="required___yY_P2">*</span>
                                        <span>First name</span>
                                    </label>
                                </div>
                            </div>
                            <div>
                                <div className="component-input___HBr7j">
                                    <div>

                                        <Input
                                            value={formData.first_name}
                                            onChange={(val) => handleChange('first_name', val.target.value)}
                                            onBlur={() => handleBlur('first_name')}
                                            validationMessage={errors.first_name}
                                            invalid={!!errors.first_name}
                                        />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="mt-24">
                        <div>
                            <div className="label-wrapper___AcszK">
                                <div>
                                    <label className="label___WET63" aria-disabled="false">
                                        <span className="required___yY_P2">*</span>
                                        <span>Last name</span>
                                    </label>
                                </div>
                            </div>
                            <div>
                                <div className="component-input___HBr7j">
                                    <div>
                                        <Input
                                            value={formData.last_name}
                                            onChange={(val) => handleChange('last_name', val.target.value)}
                                            onBlur={() => handleBlur('last_name')}
                                            validationMessage={errors.last_name}
                                            invalid={!!errors.last_name}
                                        />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="mt-24">
                        <div>
                            <div className="label-wrapper___AcszK">
                                <div>
                                    <label className="label___WET63" aria-disabled="false">
                                        <span className="required___yY_P2">*</span>
                                        <span>Email</span>
                                    </label>
                                </div>
                            </div>
                            <div>
                                <div className="component-input___HBr7j">
                                    <div>
                                        <Input
                                            value={formData.email}
                                            onChange={(val) => handleChange('email', val.target.value)}
                                            onBlur={() => handleBlur('email')}
                                            validationMessage={errors.email}
                                            invalid={!!errors.email}
                                            disabled={isSaved}
                                        />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    {showApiKey && (
                        <div className="mt-24">
                            <div>
                                <div className="label-wrapper___AcszK">
                                    <div>
                                        <label className="label___WET63" aria-disabled="false">
                                            <span className="required___yY_P2">*</span>
                                            <span>API key</span>
                                        </label>
                                    </div>
                                </div>
                                <div>
                                    <div className="component-input___HBr7j">
                                        <div>
                                            <TextArea
                                                value={formData.apiKey}
                                                onChange={(val) => handleChange('apiKey', val.target.value)}
                                                validationMessage={errors.apiKey}
                                                invalid={!!errors.apiKey}
                                                minRows={4}
                                            />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    )}
                    <div className="mt-24">
                        <button onClick={handleSubmit} className="button___Ky_Bj type-primary___KNJNo size-l___SKn9m">
                            {isSaved ? 'Save' : 'Register'}
                        </button>
                    </div>
                </div>
            </div>

            {message && (
                <CustomNotification
                    message={message}
                    type={isSuccess ? 'success' : 'error'}
                    onClose={() => setMessage(null)}
                />
            )}
        </div>
    );
};